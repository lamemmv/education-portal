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
import { hitToBrowseFile } from '../../files/upload/actions';
import NotificationContainer from '../../notify/notification.container';
import Uploader from '../../files/upload/container';
import temporaryImage from '../../../../assets/images/image.png';

class CreateNews extends Component {
    constructor(props, context) {
        super(props, context);
        this.state = {
            title: '',
            ingress: '',
            content: '',
            blobId: null,
            published: false
        };
        this.handleInputChange = this.handleInputChange.bind(this);
        this.cancel = this.cancel.bind(this);
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

    cancel = () => {
        this.context.router.history.push('/news');
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
        if (_files != null && _files.length > 0) {
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
        if (nextProps.news.redirectTo) {
            this.context.router.history.push(nextProps.news.redirectTo);
        } else {
            const { blobId, files } = nextProps.news;
            this.setState((prevState, nextState) => {
                return {
                    blobId: blobId
                }
            });

            this.validate(files);
        }
    }

    render() {
        const { createNews,
            prepareDataForCreatingNews,
            hitToBrowseFile } = this.props.actions;
        return (
            <section class='container'>
                <form role='form'>
                    <Uploader callbackAction={prepareDataForCreatingNews}
                        single={true}
                        image={true} />
                    <NotificationContainer />
                    <legend class='ep-form-title'><Text id='news.createNews'></Text></legend>
                    <div class='row'>
                        <div class='col-md-7'>
                            <div className={classNames('form-group bmd-form-group')}>
                                <label for="newsTitle" class="bmd-label-floating"><Text id='news.title'></Text></label>
                                <input type="text"
                                    name='title'
                                    class="form-control"
                                    id="newsTitle"
                                    value={this.state.title}
                                    onChange={this.handleInputChange} required />
                                <span class="bmd-help"><Text id='news.titleRequired'></Text></span>
                            </div>
                            <div class='form-group'>
                                <label for="newsIngress" class="bmd-label-floating"><Text id='news.ingress'></Text></label>
                                <input type="text"
                                    name='ingress'
                                    class="form-control"
                                    id="newsIngress"
                                    value={this.state.ingress}
                                    onChange={this.handleInputChange} />
                            </div>
                            <div className={classNames('form-group bmd-form-group')}>
                                <label for="newsContent" class="bmd-label-floating"><Text id='news.content'></Text></label>
                                <textarea class="form-control"
                                    rows="3"
                                    name='content'
                                    id='newsContent'
                                    value={this.state.content}
                                    onChange={this.handleInputChange}
                                    required ></textarea>
                                <span class="bmd-help"><Text id='news.contentRequired'></Text></span>
                            </div>
                            <span class='bmd-form-group is-filled'>
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox"
                                            name='published'
                                            checked={this.state.published}
                                            onChange={this.handleInputChange} />
                                        <span class="checkbox-decorator"><span class="check"></span>
                                            <div class="ripple-container"></div>
                                        </span>
                                        <Text id='news.published'></Text>
                                    </label>
                                </div>
                            </span>
                            <button type="button"
                                class="btn btn-primary btn-raised float-right"
                                disabled={!this.state.formValid}
                                onClick={() => createNews(this.state)}>
                                <Text id='news.create'></Text>
                            </button>
                            <button class="btn btn-secondary btn-raised float-right"
                                onClick={() => this.cancel()}>
                                <Text id='cancel'></Text>
                            </button>
                            <button type="button"
                                class="btn btn-raised btn-secondary"
                                onClick={() => hitToBrowseFile('5a55c93dcf193c134c344c81')}>
                                <Text id='files.selectImage'></Text>
                            </button>
                        </div>
                        <div class='col-md-5'>
                            <img src={temporaryImage}
                                alt="newImage"
                                style={{ width: '100%' }}
                                class="rounded float-right mx-auto d-block" />
                        </div>
                    </div>
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
        actions: bindActionCreators({ ...NewsActions, hitToBrowseFile }, dispatch)
    }
}
export default connect(mapStateToProps, mapDispatchToProps)(CreateNews);
