import { h, Component } from 'preact';
import {
    connect
} from 'preact-redux';

import {
    bindActionCreators
} from 'redux';

import { Text } from 'preact-i18n';
let classNames = require('classnames');

import * as styles from './styles.css';
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
        this.validate();
    }

    validate = (files) => {
        if (this.state.title == '') {
            this.setState({
                formValid: false
            });
            return;
        }

        if (this.state.content == '') {
            this.setState({
                formValid: false
            });
            return;
        }
        let _files = files ? files : this.props.news.files;
        if (_files != null && _files.length > 0){
            this.setState({
                formValid: false
            });
            return;
        }

        this.setState({
            formValid: true
        });
    }

    componentWillReceiveProps(nextProps) {
        const { blobId, files } = nextProps.news;
        this.setState((prevState, nextState) => {
            return {
                blobId: blobId
            }
        });

        this.validate(files);
    }

    render() {
        const { createNews } = this.props.actions;
        return (
            <section class='ep-container'>
                <form role='form'>
                    <NotificationContainer />
                    <Uploader callbackAction={NewsActions.prepareDataForCreatingNews} single={true} />
                    <div className={classNames('form-group', { 'has-error': this.state.title=='' })}>
                        <input type="text"
                            name='title'
                            class="form-control"
                            id="newsTitle"
                            value={this.state.title}
                            onChange={this.handleInputChange} required />
                        <span class="form-highlight"></span>
                        <span class="form-bar"></span>
                        <label for="newsTitle"><Text id='news.title'></Text></label>
                    </div>
                    <div class='form-group'>
                        <input type="text"
                            name='ingress'
                            class="form-control"
                            id="newsIngress"
                            value={this.state.ingress}
                            onChange={this.handleInputChange} />
                        <span class="form-highlight"></span>
                        <span class="form-bar"></span>
                        <label for="newsIngress"><Text id='news.ingress'></Text></label>
                    </div>
                    <div className={classNames('form-group', { 'has-error': this.state.content=='' })}>
                        <textarea class="form-control"
                            rows="3"
                            name='content'
                            id='newsContent'
                            value={this.state.content}
                            onChange={this.handleInputChange}
                            required ></textarea>
                        <span class="form-highlight"></span>
                        <span class="form-bar"></span>
                        <label for="newsContent"><Text id='news.content'></Text></label>
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
                                tabindex="3"></span><Text id='news.published'></Text></label>
                    </div>
                    <button type="button"
                        class="mdc-button mdc-button--raised"
                        disabled={!this.state.formValid}
                        onClick={() => createNews(this.state)}><Text id='news.create'></Text></button>
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
