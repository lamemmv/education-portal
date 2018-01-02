import { h, Component } from 'preact';
import {
    connect
} from 'preact-redux';

import {
    bindActionCreators
} from 'redux';

import { Carousel } from 'react-responsive-carousel';
import * as NewsActions from '../../news/newsActions';
import * as styles from './styles.css';
import API from '../../api';

class ShortNews extends Component {
    componentWillMount() {
        this.props.actions.getNews({ page: 1, size: 6 });
    }

    render() {
        let { items } = this.props.news;
        return (
            <Carousel
                showThumbs={false}
                autoPlay={true}
                showStatus={false}
                transitionTime={1000}
                interval={5000}>
                {items.map((item) => {
                    return (
                        <div class="mdc-card ep-card">
                            <div class="mdc-card__horizontal-block">
                                <section class="mdc-card__primary">
                                    <h1 class="mdc-card__title mdc-card__title--large">{item.title}</h1>
                                    <h2 class="mdc-card__subtitle">{item.ingress} </h2>
                                </section>
                                {
                                    item.blob ?
                                        (<img class="mdc-card__media-item" src={API.getServerDomain() + item.blob.virtualPath} />)
                                        : null
                                }
                            </div>
                            <section class="mdc-card__actions">
                                <button class="mdc-button mdc-button--compact mdc-card__action">Chi tiết >></button>
                            </section>
                        </div>
                    )
                })}
            </Carousel>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        news: state.news
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(NewsActions, dispatch)
    }
}


export default connect(mapStateToProps, mapDispatchToProps)(ShortNews);