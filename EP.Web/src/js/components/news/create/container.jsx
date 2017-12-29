import { h, Component } from 'preact';
import {
    connect
} from 'preact-redux';

import {
    bindActionCreators
} from 'redux';

import * as styles from './styles.css';
let classNames = require('classnames');
import * as NewsActions from '../newsActions';
import Uploader from '../../files/upload/container';
import NotificationContainer from '../../notify/notification.container';

class CreateNews extends Component {
    constructor(props) {
        super(props);
        this.state = {
            title: '',
            ingress: '',
            content: '',
            blobId: null,
            published: false
        };
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

    componentWillReceiveProps(nextProps) {
        const { blobId } = nextProps.news;
        this.setState((prevState, nextState) => {
            return {
                blobId: blobId
            }
        })
    }

    render() {
        const { createNews } = this.props.actions;
        return (
            <section class='ep-container'>
                <form role='form'>
                    <NotificationContainer />
                    <Uploader callbackAction={NewsActions.prepareDataForCreatingNews} />
                    <div class="form-group">
                        <input type="text"
                            name='title'
                            class="form-control"
                            id="newsTitle"
                            value={this.state.title}
                            onChange={this.handleInputChange} />
                        <span class="form-highlight"></span>
                        <span class="form-bar"></span>
                        <label for="newsTitle">Title</label>
                    </div>
                    <div class="form-group">
                        <input type="text"
                            name='ingress'
                            class="form-control"
                            id="newsIngress"
                            value={this.state.ingress}
                            onChange={this.handleInputChange} />
                        <span class="form-highlight"></span>
                        <span class="form-bar"></span>
                        <label for="newsIngress">Ingress</label>
                    </div>
                    <div class="form-group">
                        <textarea class="form-control"
                            rows="3"
                            name='content'
                            id='newsContent'
                            value={this.state.content}
                            onChange={this.handleInputChange}></textarea>
                        <span class="form-highlight"></span>
                        <span class="form-bar"></span>
                        <label for="newsContent">Content</label>
                    </div>
                    <div class="form-group checkbox">
                        <input type="checkbox"
                            id="newsPublished"
                            name='published'
                            checked={this.state.published}
                            onChange={this.handleInputChange} />
                        <label for="newsPublished">
                            <span class="chk-span"
                                className={classNames('chk-span', { 'checked': this.state.published })}
                                tabindex="3"></span>Published</label>
                    </div>
                    <button type="button"
                        class="mdc-button mdc-button--raised"
                        onClick={() => createNews(this.state)}>Create</button>
                </form>
            </section>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        news: state.newsCreate
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(NewsActions, dispatch)
    }
}
export default connect(mapStateToProps, mapDispatchToProps)(CreateNews);
