import { h, Component } from 'preact';
import { Route, HashRouter, Switch } from 'react-router-dom';

import Home from './home/home';
import FileIndex from './files/index';
import FilePreview from './files/preview';
import NewsIndex from './news/index';
import NewsDetail from './news/detail';
import CreateNews from './news/create/container';
import EPBreadcrumbs from './breadcrumbs/container';
import RouterContainer from './breadcrumbs/router.container';

class Main extends Component {
    render() {
        return (
            <main>
                <EPBreadcrumbs />
                <div class="mdc-toolbar-fixed-adjust">
                    <HashRouter>
                        <RouterContainer>
                            <Switch>
                                <Route exact path='/' component={Home} />
                                <Route exact path='/files' component={FileIndex} />
                                <Route path='/file/:id' component={FilePreview} />
                                <Route exact path='/news' component={NewsIndex} />
                                <Route exact path='/news/create' component={CreateNews} />
                                <Route exact path='/news/:id' component={NewsDetail} />                                
                            </Switch>
                        </RouterContainer>
                    </HashRouter>
                </div>
            </main>
        );
    };
}

export default Main;
