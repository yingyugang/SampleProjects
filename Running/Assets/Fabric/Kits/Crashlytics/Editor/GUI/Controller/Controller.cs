namespace Fabric.Internal.Crashlytics.Editor.Controller
{
	using UnityEngine;
	using UnityEditor;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Timers;
	using Fabric.Internal.Editor;
	using Fabric.Internal.Editor.Controller;
	using Fabric.Internal.Editor.Detail;
	using Fabric.Internal.Editor.Model;
	using Fabric.Internal.Editor.View;
	using Fabric.Internal.Editor.View.Templates;
	
	internal class Controller : KitController
	{
		#region Pages
		private Page prefab = null;
		private Page Prefab
		{
			get {
				if (prefab == null) {
					prefab = new PrefabPage (AdvanceToValidationPage (), PrefabName, typeof (Fabric.Internal.Crashlytics.CrashlyticsInit));

					Fabric.Internal.Editor.Update.PeriodicPinger.Enqueue (new Fabric.Internal.Editor.Analytics.Events.PageViewEvent {
						ScreenName = "PrefabPage (Crashlytics)",
					});
				}
				return prefab;
			}
		}

		private Page instructions = null;
		private Page Instructions
		{
			get {
				if (instructions == null) {
					instructions = new InstructionsPage (ApplyKitChanges (), BackToKitSelector (), new List<string> () {
						"◈ Set execution order of Fabric scripts",
						"◈ Replace application class in top-level AndroidManifest.xml",
						"◈ Inject metadata in Fabric's AndroidManifest.xml"
					});

					Fabric.Internal.Editor.Update.PeriodicPinger.Enqueue (new Fabric.Internal.Editor.Analytics.Events.PageViewEvent {
						ScreenName = "InstructionsPage (Crashlytics)",
					});
				}
				return instructions;
			}
		}

		private Page dashboard = null;
		private Page Dashboard
		{
			get {
				if (dashboard == null) {
					dashboard = new View.DashboardPage (DownloadIcon (), FetchDashboardUrl (), BackToKitSelector ());

					Fabric.Internal.Editor.Update.PeriodicPinger.Enqueue (new Fabric.Internal.Editor.Analytics.Events.PageViewEvent {
						ScreenName = "DashboardPage (Crashlytics)",
					});
				}
				return dashboard;
			}
		}
		#endregion
		
		private bool isDownloadingIcon = false;
		// These are retrieved in a timer, on a non-main thread. We can't assign anything to
		// Settings.Instance from any thread but the main thread. These serve as a cache.
		private Texture2D Icon = null;

		private const string PrefabName = "CrashlyticsInit";
		public const string Name = "Crashlytics";

		public Controller(Fabric.Internal.Editor.API.V1 api)
		{
		}

		public Version Version()
		{
			return Fabric.Internal.Crashlytics.Editor.Info.Version;
		}
		
		public KitControllerStatus PageFromState(out Page page)
		{
			if (Settings.Instance.InstalledKits.Exists (kit => kit.Name.Equals (Name) && kit.Installed)) {
				page = Dashboard;
				return KitControllerStatus.NextPage;
			}
			
			page = null;
			switch (Settings.Instance.FlowSequence) {
			case 0:
				page = Instructions;
				break;
			case 1:
				page = Prefab;
				break;
			case 2:
				return KitControllerStatus.LastPage;
			}
			
			return KitControllerStatus.NextPage;
		}

		public string DisplayName()
		{
			// Same as 'Name' by coincidence
			return "Crashlytics";
		}
		
		#region AdvanceToValidationPage
		private Action AdvanceToValidationPage()
		{
			return delegate() {
				Settings.Instance.FlowSequence = 2;

				Fabric.Internal.Editor.Update.PeriodicPinger.Enqueue (new Fabric.Internal.Editor.Analytics.Events.PageViewEvent {
					ScreenName = "ValidationPage (Crashlytics)",
				});
			};
		}
		#endregion
		
		#region ApplyInstructions
		private Action ApplyKitChanges()
		{
			return delegate() {
				List<Settings.InstalledKit> installedKits = Settings.Instance.InstalledKits;

				installedKits.RemoveAll (installed => installed.Name.Equals (Name));
				installedKits.Add (new Settings.InstalledKit {
					Name = Name,
					Installed = false
				});

				Settings.Instance.InstalledKits = installedKits;

				CrashlyticsSetup.EnableCrashlytics (false);
				Settings.Instance.FlowSequence = 1;
			};
		}
		#endregion
		
		#region BackToOrganizations
		private static Action BackToKitSelector()
		{
			return delegate() {
				Settings.Instance.Kit = null;
			};
		}
		#endregion
		
		#region DownloadIcon
		public Func<Texture2D> DownloadIcon()
		{
			return delegate() {
				if (!isDownloadingIcon && Icon == null && !string.IsNullOrEmpty (Settings.Instance.IconUrl)) {
					isDownloadingIcon = true;

					new AsyncTaskRunnerBuilder<byte[]> ().Do ((object[] args) => {
						return Internal.Editor.Net.Validator.MakeRequest (() => {
							return Internal.Editor.API.V1.DownloadFile (Settings.Instance.IconUrl);
						});
					}).OnError ((System.Exception e) => {
						Utils.Warn ("App icon download failed. {0}", e.Message);
						isDownloadingIcon = false;
						return AsyncTaskRunner<byte[]>.ErrorRecovery.Nothing;
					}).OnCompletion ((byte[] textureData) => {
						Texture2D texture = new Texture2D (0, 0);
						texture.LoadImage (textureData);
						Icon = texture;
						isDownloadingIcon = false;
					}).Run ();
				}
				
				return Icon;
			};
		}
		#endregion
		
		#region FetchDashboardUrl
		public static Func<string> FetchDashboardUrl()
		{
			return delegate() {
				return Settings.Instance.DashboardUrl;
			};
		}
		#endregion
	}
}