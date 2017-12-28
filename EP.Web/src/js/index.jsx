import {
    h,
    render
} from 'preact';
import { IntlProvider } from 'preact-i18n';
import definition from './locales/en.json';

import App from './components/App.jsx';
import 'material-components-web/dist/material-components-web';
import 'material-components-web/dist/material-components-web.css';
import { MDCToolbar, MDCToolbarFoundation } from '@material/toolbar';
import configureStore from './store/configureStore';

const store = configureStore();

if (process.env.NODE_ENV !== 'production') {
    console.log('Looks like we are in development mode!');
}

const renderApp = (rootId = 'root') => {
    const targetEl = document.getElementById(rootId);

    render((<IntlProvider definition={definition}>
        <App store={store} />
    </IntlProvider>), targetEl);
}

renderApp();

//fixed last row of toolbar
(function () {
    var pollId = 0;
    pollId = setInterval(function () {
        var pos = getComputedStyle(document.querySelector('.mdc-toolbar')).position;
        if (pos === 'fixed' || pos === 'relative') {
            init();
            clearInterval(pollId);
        }
    }, 250);

    function init() {
        var toolbarEl = document.querySelector('.mdc-toolbar')
        var toolbar = new MDCToolbar(toolbarEl);
        toolbar.listen('MDCToolbar:change', function (evt) {
            var flexibleExpansionRatio = evt.detail.flexibleExpansionRatio;
        });
        toolbar.fixedAdjustElement = document.querySelector('.mdc-toolbar-fixed-adjust');
    }
})();