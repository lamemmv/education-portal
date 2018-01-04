import { h, Component } from 'preact';
import { Route, HashRouter, Switch } from 'react-router-dom';
import { Text } from 'preact-i18n';

import Home from './home/home';
import Files from './files/fileList';
import FilePreview from './files/preview';
import NewsIndex from './news/index';
import NewsDetail from './news/detail';
import CreateNews from './news/create/container';
import EPBreadcrumbs from './breadcrumbs/container';
import RouterContainer from './breadcrumbs/router.container';
import demoimages from '../../assets/images/teaching-methods.jpg';

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
                            <Route path='/file/:id' component={FilePreview} />
                            <Route exact path='/news' component={NewsIndex} />
                            <Route exact path='/news/create' component={CreateNews} />
                            <Route exact path='/news/:id' component={NewsDetail} />
                        </Switch>
                    </RouterContainer>
                </HashRouter>
                {/* <div class="mdc-toolbar-fixed-adjust">
                    <div class='mdc-layout-grid'>
                        <div class='mdc-layout-grid__inner' >
                            <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-2 ep-left">
                                <div class="mdc-card demo-card">
                                    <section class="mdc-card__media demo-card__16-9-media"></section>
                                    <section class="mdc-card__primary">
                                        <h1 class="mdc-card__title mdc-card__title--large">Các cơ sở liên kết</h1>
                                        <h2 class="mdc-card__subtitle">Subtitle here</h2>
                                    </section>
                                    <section class="mdc-card__actions ep-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action">Chi tiết</button>
                                    </section>
                                </div>
                                <div class="mdc-card demo-card">
                                    <section class="mdc-card__media">
                                        <img class="mdc-card__media-item"
                                            src={require('../../assets/images/books.jpg')} />
                                    </section>
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
                                            <h1 class="mdc-card__title mdc-card__title--large"><Text id='teaching-methods-and-programs'></Text></h1>
                                            <h2 class="mdc-card__subtitle" style={{ marginTop: '5px' }}>
                                                Trung tâm áp dụng phương pháp dạy tiên tiến nhất. Học viên có thể phát huy hết khả năng...</h2>
                                        </section>
                                        <img class="mdc-card__media-item" src={demoimages} />
                                    </div>
                                    <section class="mdc-card__actions ep-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action"><Text id='news.detail'></Text></button>
                                    </section>
                                </div>
                                <div class="mdc-card demo-card">
                                    <div class="mdc-card__horizontal-block">
                                        <section class="mdc-card__primary">
                                            <h1 class="mdc-card__title mdc-card__title--large">
                                                <Text id='fee-policy'></Text></h1>
                                            <h2 class="mdc-card__subtitle" style={{ marginTop: '5px' }}>
                                                Đối với học viên thường xuyên của trung tâm...</h2>
                                        </section>
                                        <img class="mdc-card__media-item" src={require('../../assets/images/money-bag.jpg')} />
                                    </div>
                                    <section class="mdc-card__actions ep-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action"><Text id='news.detail'></Text></button>
                                    </section>
                                </div>
                                <div class="mdc-card demo-card">
                                    <div class="mdc-card__horizontal-block">
                                        <section class="mdc-card__primary">
                                            <h1 class="mdc-card__title mdc-card__title--large">Nội qui trung tâm</h1>
                                            <h2 class="mdc-card__subtitle">Học viên khi tham gia học tại trung tâm phải chấp hành một số nội qui sau... </h2>
                                        </section>
                                        <img class="mdc-card__media-item" src={require('../../assets/images/rules.jpg')} />
                                    </div>
                                    <section class="mdc-card__actions ep-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action"><Text id='news.detail'></Text></button>
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
                                    <section class="mdc-card__actions ep-card__actions">
                                        <button class="mdc-button mdc-button--compact mdc-card__action"><Text id='news.detail'></Text></button>
                                    </section>
                                </div>
                            </div>
                        </div>
                    </div>

                </div> */}
            </main>
        );
    };
}

export default Main;
