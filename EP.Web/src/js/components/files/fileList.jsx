import { h, Component } from "preact";
import { Link } from 'react-router-dom';
import { connect } from 'preact-redux';
import { Localizer, Text } from 'preact-i18n';
let classNames = require('classnames');
import API from '../api';

import {
    getFiles,
    selectFolder,
    deselectFolder
} from './fileActions';

import FileMenu from './menu/container';
import Pagination from './pagination/container';
import * as styles from './styles.css';
import Utils from '../utils';
import NotificationContainer from '../notify/notification.container';
import Spinner from '../spinner/container';

class FileList extends Component {

    constructor(props) {
        super(props);
        this.state = {
            selected: false
        };
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleInputClick = this.handleInputClick.bind(this);
        this.triggerSelection = this.triggerSelection.bind(this);
    }

    handleInputChange(event, node) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const { selectFolder, deselectFolder } = this.props;
        if (value) {
            selectFolder(node);
        } else {
            deselectFolder(node);
        }
    }

    handleInputClick(event) {
        event.stopPropagation();
    }

    triggerSelection(event, node) {
        event.stopPropagation();
        $(`#${node.id}`).trigger('click');
        event.preventDefault();
    }

    componentWillReceiveProps(nextProps, prevProps) {
        if (nextProps.fileState.downloadFile) {
            window.location = `${API.getBaseUri()}admin/blobManager/File/${nextProps.fileState.downloadFile}`
        } else {
            if (nextProps.match.params) {
                if (nextProps.match.params.id != this.props.match.params.id) {
                    this.init(nextProps.match.params.id);
                }
            }
        }
    }

    componentWillMount() {
        if (this.props.match.params.id != null) {
            this.init(this.props.match.params.id);
        } else {
            this.init();
        }
    }

    init = (folderId) => {
        if (!this.props.fileState.files) {
            this.props.fileState.files = [];
        }
        this.props.getFiles({
            page: this.props.fileState.currentPage,
            folderId: folderId
        });
    }

    renderTableFooter(pages, currentPage, showPagination) {
        return (
            showPagination ? (
                <Pagination />
            ) : null
        );
    }

    renderNode = (node) => {
        let nodeItem = (
            <div className={classNames('card clearfix ep-card', { 'selected': node.selected })}>
                {
                    node.nodeType == 1 ?
                        <img class="img-fluid card-img-top"
                            src={require('../../../assets/images/480px-Icons8_flat_folder.png')} />
                        :
                        <img class="img-fluid card-img-top" width={30}
                            src={Utils.getIcon(node)} />
                }
                <div class="ep-checkbox selection">
                    <input id={node.id}
                        type="checkbox"
                        name='selectNode'
                        checked={node.selected}
                        onChange={(event) => this.handleInputChange(event, node)}
                        onClick={this.handleInputClick} />
                    <label for="selectNode"
                        onClick={(event) => this.triggerSelection(event, node)}></label>
                </div>
                <div class='card-block'>
                    <div class='card-text ep-card-text'>{node.name}</div>
                </div>
            </div>);

        return (
            <div class="col-2 ep-node-item" title={node.name}>
                {node.nodeType == 1 ?
                    <Link to={`/files/${node.id}`}>{nodeItem}</Link>
                    : nodeItem
                }
            </div>
        );
    }

    render() {
        const {
            files,
            currentPage,
            pages,
            showPagination,
            blob,
            loading
        } = this.props.fileState;

        return (
            <section class='container'>
                <Spinner loading={loading} />
                <NotificationContainer />
                {this.props.match.params.id ? <FileMenu params={blob} /> : null}
                <div class='row'>
                    {
                        files.map((node) => {
                            return this.renderNode(node, selectFolder)
                        })
                    }
                </div>
            </section>
        );
    };
}

const mapStateToProps = (state) => {
    return {
        fileState: state.posts.fileState
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        getFiles: (filter) => {
            dispatch(getFiles(filter));
        },
        selectFolder: (node) => {
            dispatch(selectFolder(node));
        },
        deselectFolder: (node) => {
            dispatch(deselectFolder(node));
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(FileList);
