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

import * as styles from './styles.css';

class Main extends Component {
    render() {
        return (
            <main>
                <EPBreadcrumbs />
                <div class="mdc-toolbar-fixed-adjust" style={{marginTop: '312px'}}>
                    <div class='mdc-layout-grid'>
                        <div class='mdc-layout-grid__inner' >
                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-2 ep-left"></div>
                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-7 ep-center">
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
                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-3 ep-right"></div>
                        </div>
                    </div>

                </div>
            </main>
        );
    };
}

export default Main;
