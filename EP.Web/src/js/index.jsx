import {
    h,
    render
} from 'preact';
import { IntlProvider } from 'preact-i18n';

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