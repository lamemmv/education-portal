import {
    h,
    render
} from 'preact';
import { IntlProvider } from 'preact-i18n';

// import 'material-components-web/dist/material-components-web';
// import 'material-components-web/dist/material-components-web.css';
// import { MDCToolbar, MDCToolbarFoundation } from '@material/toolbar';
import styles from 'react-responsive-carousel/lib/styles/carousel.min.css';

import 'bootstrap';
import 'bootstrap-material-design/dist/css/bootstrap-material-design.min.css';

import definition from './locales/vi.json';
import App from './components/App.jsx';
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

// //fixed last row of toolbar
// (function () {
//     var pollId = 0;
//     pollId = setInterval(function () {
//         var pos = getComputedStyle(document.querySelector('.mdc-toolbar')).position;
//         if (pos === 'fixed' || pos === 'relative') {
//             init();
//             clearInterval(pollId);
//         }
//     }, 250);

//     function init() {
//         var toolbarEl = document.querySelector('.mdc-toolbar')
//         var toolbar = new MDCToolbar(toolbarEl);
//         toolbar.listen('MDCToolbar:change', function (evt) {
//             var flexibleExpansionRatio = evt.detail.flexibleExpansionRatio;
//         });
//         toolbar.fixedAdjustElement = document.querySelector('.mdc-toolbar-fixed-adjust');
//     }
// })();


// //fixed last row of toolbar
(function () {
    $(window).on('scroll', function(event) {
        var scrollValue = $(window).scrollTop();
        if (scrollValue == settings.scrollTopPx || scrollValue > 70) {
             $('.navbar').addClass('navbar');
        } 
    });
})();