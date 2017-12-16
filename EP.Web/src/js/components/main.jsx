import React, { Component } from 'react';
import { Route, HashRouter, Switch } from 'react-router-dom';
import Home from './home/home';
import FileList from './files/list';
import FilePreview from './files/preview';
import Schedule from './schedule/schedule';

class Main extends Component {
    render(){
        return (
            <HashRouter>
                <Switch>
                    <Route exact path='/' component={Home} />
                    <Route exact path='/files' component={FileList} />
                    <Route path='/files/:id' component={FilePreview} />
                    <Route path='/schedule' component={Schedule} />
                </Switch>
            </HashRouter>
        );
    };
} 

export default Main;
