import { h, Component } from 'preact';
import { Route, HashRouter, Switch } from 'react-router-dom';
import { Text } from 'preact-i18n';

import Home from './home/home';
import Files from './files/fileList';
import NewsIndex from './news/index';
import NewsDetail from './news/detail';
import CreateNews from './news/create/container';
import EPBreadcrumbs from './breadcrumbs/container';
import RouterContainer from './breadcrumbs/router.container';

import * as styles from './styles.css';

class Main extends Component {
    render() {
        return (
            <main>
                <EPBreadcrumbs />
                <HashRouter>
                    <RouterContainer>
                        <Switch>
                            <Route exact path='/' component={Home} />
                            <Route exact path='/files' component={Files} />
                            <Route exact path='/files/:id' component={Files} />
                            <Route exact path='/news' component={NewsIndex} />
                            <Route exact path='/news/create' component={CreateNews} />
                            <Route exact path='/news/:id' component={NewsDetail} />
                        </Switch>
                    </RouterContainer>
                </HashRouter>
            </main>
        );
    };
}

export default Main;
