import React, { Component } from 'react';
import { render } from 'react-dom';
import App from './components/App.jsx';
import '../../semantic/dist/semantic.min.css';
import configureStore from './store/configureStore';

const store = configureStore();

if (process.env.NODE_ENV !== 'production') {
    console.log('Looks like we are in development mode!');
}

render(<App store={store} />, document.getElementById('root'));