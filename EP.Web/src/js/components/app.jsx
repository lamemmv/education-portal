import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Provider } from 'react-redux';
import EPHeader from './header';

import { Route, HashRouter, Switch } from 'react-router-dom';
import Home from './home/home';
import FileIndex from './posts/index';
import FilePreview from './files/preview';
import NewsIndex from './news/index';
import CreateNews from './news/create/news.create';

const App = ({ store }) => (
    <Provider store={store}>
        <div>
            <EPHeader />
            <HashRouter>
                <Switch>
                    <Route exact path='/' component={Home} />
                    <Route exact path='/files' component={FileIndex} />
                    <Route path='/files/:id' component={FilePreview} />
                    <Route exact path='/news' component={NewsIndex} />
                    <Route exact path='/news/create' component={CreateNews} />
                </Switch>
            </HashRouter>
        </div>
    </Provider>
)

App.propTypes = {
    store: PropTypes.object.isRequired,
}

export default App