import {
    h,
    render
} from 'preact';
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

    render((<App store={store} />), targetEl);
}

renderApp();