import React, { Component } from 'react';
import { render } from 'react-dom';
import App from './components/App.jsx';

if (process.env.NODE_ENV !== 'production') {
    console.log('Looks like we are in development mode!');
}

render(<App />, document.getElementById('root'));