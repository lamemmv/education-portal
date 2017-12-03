import React, { Component } from 'react';
import { render } from 'react-dom';
import EPHeader from './header';
import Main from './main';

export default class App extends Component {
    constructor(props, context) {
        super(props, context);
    }

    render() {
        return (
            <div>
                <EPHeader />
                <Main />
            </div>
        );
    }
}