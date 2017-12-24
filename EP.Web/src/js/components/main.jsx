import { h, Component } from 'preact';
import { Route, HashRouter, Switch } from 'react-router-dom';

import Home from './home/home';
import FileIndex from './files/index';
import FilePreview from './files/preview';
import NewsIndex from './news/index';
import CreateNewsIndex from './news/create/index';

class Main extends Component {
    render() {
        return (
            <main>
                <div class="mdc-toolbar-fixed-adjust">
                    <HashRouter>
                        <Switch>
                            <Route exact path='/' component={Home} />
                            <Route exact path='/files' component={FileIndex} />
                            <Route path='/files/:id' component={FilePreview} />
                            <Route exact path='/news' component={NewsIndex} />
                            <Route exact path='/news/create' component={CreateNewsIndex} />
                        </Switch>
                    </HashRouter>
                </div>
            </main>
        );
    };
}

export default Main;
