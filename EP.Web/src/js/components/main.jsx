import { h, Component } from 'preact';
import { Route, HashRouter, Switch } from 'react-router-dom';

import Home from './home/home';
import Files from './files/fileList';
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
                                    <section class="mdc-card__primary">
                                        <h1 class="mdc-card__title mdc-card__title--large">Các cơ sở liên kết</h1>
                                        <h2 class="mdc-card__subtitle">Subtitle here</h2>
                                    </section>
                                    <section class="mdc-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 1</button>
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 2</button>
                                    </section>
                                </div>
                                <div class="mdc-card demo-card">
                                    <section class="mdc-card__media demo-card__16-9-media"></section>
                                    <section class="mdc-card__supporting-text">
                                        Hình sách
                                    </section>
                                </div>

                            </div>
                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-7 ep-center">
                                <HashRouter>
                                    <RouterContainer>
                                        <Switch>
                                            <Route exact path='/' component={Home} />
                                            <Route exact path='/files' component={Files} />
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
                                            <h1 class="mdc-card__title mdc-card__title--large">Phương pháp & chường trình dạy</h1>
                                            <h2 class="mdc-card__subtitle">Phương pháp dạy</h2>
                                        </section>
                                        <img class="mdc-card__media-item" src={demoimages} />
                                    </div>
                                    <section class="mdc-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 1</button>
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 2</button>
                                    </section>
                                </div>
                                <div class="mdc-card demo-card">
                                    {/* <div class="mdc-card__horizontal-block">
                                        <section class="mdc-card__primary">
                                            <h1 class="mdc-card__title mdc-card__title--large">Chương trình dạy</h1>
                                            <h2 class="mdc-card__subtitle">Subtitle here</h2>
                                        </section>
                                        <img class="mdc-card__media-item mdc-card__media-item--2x" src={demoimages} />
                                    </div>
                                    <section class="mdc-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 1</button>
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Action 2</button>
                                    </section> */}
                                    <div class="mdc-card__horizontal-block">
                                        <section class="mdc-card__primary">
                                            <h1 class="mdc-card__title mdc-card__title--large">Qui định học phí</h1>
                                            <h2 class="mdc-card__subtitle">Phương pháp dạy</h2>
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
                                            <h1 class="mdc-card__title mdc-card__title--large">Nội qui trung tâm</h1>
                                            <h2 class="mdc-card__subtitle">Ưu tiên </h2>
                                        </section>
                                        <img class="mdc-card__media-item" src={demoimages} />
                                    </div>
                                    <section class="mdc-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Chi tiết >></button>
                                    </section>
                                </div>
                                <div class="mdc-card demo-card">
                                    <div class="mdc-card__horizontal-block">
                                        <section class="mdc-card__primary">
                                            <h1 class="mdc-card__title mdc-card__title--large">Chính sách ưu đãi</h1>
                                            <h2 class="mdc-card__subtitle">Ưu tiên </h2>
                                        </section>
                                        <img class="mdc-card__media-item" src={demoimages} />
                                    </div>
                                    <section class="mdc-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Chi tiết >></button>
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
