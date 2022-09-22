import { Modal } from "antd";
import { Component } from "react";
import { Model } from "../context/ModelContext";

type ForgeModelViewModalProps = {
  model: Model | undefined;
  onClose: () => void;
};

class ForgeModelViewModal extends Component<ForgeModelViewModalProps> {
  viewer?: Autodesk.Viewing.GuiViewer3D;

  constructor(props: ForgeModelViewModalProps) {
    super(props);
  }

  render() {
    return (
      <Modal
        open={this.props.model !== undefined}
        onCancel={() => this.props.onClose()}
        onOk={() => this.props.onClose()}
        bodyStyle={{ height: "750px" }}
        width={1400}
        footer={null}
        destroyOnClose
      >
        <div className="Viewer" id="forgeViewer" />
      </Modal>
    );
  }

  public componentDidMount() {
    if (!window.Autodesk) {
      this.loadCss(
        "https://developer.api.autodesk.com/modelderivative/v2/viewers/7.*/style.min.css"
      );

      this.loadScript(
        "https://developer.api.autodesk.com/modelderivative/v2/viewers/7.*/viewer3D.min.js"
      ).onload = () => {
        this.onScriptLoaded();
      };
    }
  }

  public loadCss(src: string): HTMLLinkElement {
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = src;
    link.type = "text/css";
    document.head.appendChild(link);
    return link;
  }

  private loadScript(src: string): HTMLScriptElement {
    const script = document.createElement("script");
    script.type = "text/javascript";
    script.src = src;
    script.async = true;
    script.defer = true;
    document.body.appendChild(script);
    return script;
  }

  private onScriptLoaded() {
    fetch("https://localhost:5001/token")
      .then((response) => {
        if (response.ok) {
          return response.json();
        }
        throw response;
      })
      .then((token) => {
        let that: any = this;
        var options = {
          env: "AutodeskProduction",
          accessToken: token.accessToken,
        };
        var documentId: string = "urn:" + this.props.model?.derivativeId;
        Autodesk.Viewing.Initializer(options, function onInitialized() {
          Autodesk.Viewing.Document.load(
            documentId,
            that.onDocumentLoadSuccess.bind(that),
            that.onDocumentLoadError
          );
        });
      });
  }

  async onDocumentLoadSuccess(doc: Autodesk.Viewing.Document) {
    console.log(doc.getRoot());
    // A document contains references to 3D and 2D viewables.
    var items = doc.getRoot().search({
      type: "geometry",
      role: "3d",
    });
    if (items.length === 0) {
      console.error("Document contains no viewables.");
      return;
    }

    var viewerDiv: any = document.getElementById("forgeViewer");
    this.viewer = new Autodesk.Viewing.GuiViewer3D(viewerDiv);
    this.viewer.start();

    // loading it dynamically
    const { AutodeskViewingExtension } = await import(
      "./AutodeskViewingExtension"
    );
    AutodeskViewingExtension.register();
    this.viewer.loadExtension("MyExtension");

    var options2 = {};
    let that: any = this;
    this.viewer
      .loadDocumentNode(doc, items[1], options2)
      .then(function (model1: Autodesk.Viewing.Model) {
        var options1: any = {};
        options1.keepCurrentModels = true;

        that.viewer
          .loadDocumentNode(doc, items[0], options1)
          .then(function (model2: Autodesk.Viewing.Model) {
            let extensionConfig: any = {};
            extensionConfig["mimeType"] = "application/vnd.autodesk.revit";
            extensionConfig["primaryModels"] = [model1];
            extensionConfig["diffModels"] = [model2];
            extensionConfig["diffMode"] = "overlay";
            extensionConfig["versionA"] = "2";
            extensionConfig["versionB"] = "1";

            that.viewer
              .loadExtension("Autodesk.DiffTool", extensionConfig)
              .then((res: any) => {
                console.log(res);
              })
              .catch(function (err: any) {
                console.log(err);
              });
          });
      });
  }
}

export default ForgeModelViewModal;
