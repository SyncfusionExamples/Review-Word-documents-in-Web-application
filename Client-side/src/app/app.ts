
import { DocumentEditorContainer, Toolbar } from '@syncfusion/ej2-documenteditor';
import { TitleBar } from './title-bar';

/**
 * Default document editor sample
 */

    
    let hostUrl: string = 'https://services.syncfusion.com/js/production/';

    let container: DocumentEditorContainer = new DocumentEditorContainer({ enableToolbar: true });
    DocumentEditorContainer.Inject(Toolbar);
    container.serviceUrl = hostUrl + 'api/documenteditor/';
    container.created = (): void => {
        setInterval(() => {
          updateDocumentEditorSize();
        }, 100);
        //Adds event listener for browser window resize event.
        window.addEventListener('resize', onWindowResize);
      };
    container.appendTo('#container');
    let titleBar: TitleBar = new TitleBar(document.getElementById('documenteditor_titlebar'), container.documentEditor, true);
    container.documentEditor.open(JSON.stringify((titleBar.data)));
    container.documentEditor.documentName = 'Getting Started';
    titleBar.updateDocumentTitle();
   
    container.documentChange = (): void => {
        titleBar.updateDocumentTitle();
        container.documentEditor.focusIn();
    };

    function onWindowResize() {
        //Resizes the document editor component to fit full browser window automatically whenever the browser resized.
        updateDocumentEditorSize();
      }
      function updateDocumentEditorSize() {
        //Resizes the document editor component to fit full browser window.
        var windowWidth = window.innerWidth;
        var windowHeight = window.innerHeight - titleBar.getHeight();
        container.resize(windowWidth, windowHeight);
      }
