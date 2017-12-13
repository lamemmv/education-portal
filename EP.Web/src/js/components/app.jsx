import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Provider } from 'react-redux';
import EPHeader from './header';

import { Route, HashRouter } from 'react-router-dom';
import Home from './home/home';
import FileIndex from './posts/index';
import FilePreview from './files/preview';
import NewsIndex from './news/index';

const App = ({ store }) => (
    <Provider store={store}>
        <div>
            <EPHeader />
            <HashRouter>
                <div>
                    <Route exact path='/' component={Home} />
                    <Route exact path='/files' component={FileIndex} />
                    <Route path='/files/:id' component={FilePreview} />
                    <Route path='/news' component={NewsIndex} />
                </div>
            </HashRouter>
        </div>
    </Provider>
)

App.propTypes = {
    store: PropTypes.object.isRequired,
}

export default App