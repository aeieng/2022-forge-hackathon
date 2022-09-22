export class AutodeskViewingExtension extends Autodesk.Viewing.Extension {
  load() {
    console.log("AutodeskViewingExtension has been loaded");
    return true;
  }

  unload() {
    console.log("AutodeskViewingExtension has been unloaded");
    return true;
  }

  static register() {
    Autodesk.Viewing.theExtensionManager.registerExtension(
      "MyExtension",
      AutodeskViewingExtension
    );
  }
}
