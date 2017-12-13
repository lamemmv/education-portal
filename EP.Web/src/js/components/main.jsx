import React, { Component } from 'react';
import { Route, HashRouter } from 'react-router-dom';
import Home from './home/home';
import FileList from './files/list';
import FilePreview from './files/preview';
import Schedule from './schedule/schedule';

class Main extends Component {
    render(){
        return (
            <HashRouter>
                <div>
                    <Route exact path='/' component={Home} />
                    <Route exact path='/files' component={FileList} />
                    <Route path='/files/:id' component={FilePreview} />
                    <Route path='/schedule' component={Schedule} />
                </div>
            </HashRouter>
        );
    };
} 

export default Main;
