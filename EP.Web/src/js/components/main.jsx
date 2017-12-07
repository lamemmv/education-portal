import React, { Component } from 'react';
import { Route, HashRouter } from 'react-router-dom';
import Home from './home/home';
import FileList from './files/list';
import FilePreview from './files/preview';
import Schedule from './schedule/schedule';

// The Main component renders one of the three provided
// Routes (provided that one matches). Both the /roster
// and /schedule routes will match any pathname that starts
// with /roster or /schedule. The / route will only match
// when the pathname is exactly the string "/"
class Main extends Component {
    render(){
        return (
            <HashRouter>
                <div>
                    <Route exact path='/' component={Home} />
                    <Route path='/files' component={FileList} />
                    <Route path='/files/:id' component={FilePreview} />
                    <Route path='/schedule' component={Schedule} />
                </div>
            </HashRouter>
        );
    };
} 

export default Main;
