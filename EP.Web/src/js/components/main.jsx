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
import demoimages from '../../assets/images/1-1.jpg'

import * as styles from './styles.css';

class Main extends Component {
    render() {
        return (
            <main>
                <EPBreadcrumbs />
                <div class="mdc-toolbar-fixed-adjust">
                    <div class='mdc-layout-grid'>
                        <div class='mdc-layout-grid__inner' >
                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-2 ep-left">
                                <div class="mdc-card demo-card">
                                    <section class="mdc-card__media demo-card__16-9-media"></section>
                                    <section class="mdc-card__supporting-text">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor.
                                    </section>
                                </div>
                                <div class="mdc-card demo-card">
                                    <section class="mdc-card__primary">
                                        <h1 class="mdc-card__title mdc-card__title--large">Title goes here</h1>
                                        <h2 class="mdc-card__subtitle">Subtitle here</h2>
                                    </section>
                                    <section class="mdc-card__supporting-text">
                                        Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                                        Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                                    </section>
                                    <section class="mdc-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 1</button>
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 2</button>
                                    </section>
                                </div>
                                <div class="mdc-card demo-card">
                                    <section class="mdc-card__media demo-card__16-9-media"></section>
                                    <section class="mdc-card__primary">
                                        <h1 class="mdc-card__title mdc-card__title--large">Title goes here</h1>
                                        <h2 class="mdc-card__subtitle">Subtitle here</h2>
                                    </section>
                                    <section class="mdc-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 1</button>
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 2</button>
                                    </section>
                                </div>
                            </div>
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
                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-3 ep-right">
                                <div class="mdc-card demo-card">
                                    <div class="mdc-card__horizontal-block">
                                        <section class="mdc-card__primary">
                                            <h1 class="mdc-card__title mdc-card__title--large">Title here</h1>
                                            <h2 class="mdc-card__subtitle">Subtitle here</h2>
                                        </section>
                                        <img class="mdc-card__media-item" src={demoimages} />
                                    </div>
                                    <section class="mdc-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 1</button>
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 2</button>
                                    </section>
                                </div>
                                <div class="mdc-card demo-card">
                                    <div class="mdc-card__horizontal-block">
                                        <section class="mdc-card__primary">
                                            <h1 class="mdc-card__title mdc-card__title--large">Title here</h1>
                                            <h2 class="mdc-card__subtitle">Subtitle here</h2>
                                        </section>
                                        <img class="mdc-card__media-item mdc-card__media-item--2x" src={demoimages} />
                                    </div>
                                    <section class="mdc-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 1</button>
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 2</button>
                                    </section>
                                </div>
                                <div class="mdc-card mdc-card--theme-dark demo-card demo-card--bg-demo">
                                    <section class="mdc-card__primary">
                                        <h1 class="mdc-card__title mdc-card__title--large">Title goes here</h1>
                                        <h2 class="mdc-card__subtitle">Subtitle here</h2>
                                    </section>
                                    <section class="mdc-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-button--theme-dark mdc-card__action">Action 1</button>
                                        <button class="mdc-button mdc-button--compact mdc-button--theme-dark mdc-card__action">Action 2</button>
                                    </section>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </main>
        );
    };
}

export default Main;
