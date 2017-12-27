import {
    h,
    render
} from 'preact';
import { IntlProvider } from 'preact-i18n';
import definition from './locales/en.json';

import App from './components/App.jsx';
import 'material-components-web/dist/material-components-web';
import 'material-components-web/dist/material-components-web.css';
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