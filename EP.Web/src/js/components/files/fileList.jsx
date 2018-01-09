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
        if (nextProps.match.params) {
            if (nextProps.match.params.id != this.props.match.params.id) {
                this.init(nextProps.match.params.id);
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
        const imageTypes = ['image/gif', "image/jpeg", "image/png"];
        let nodeItem = (
            <div className={classNames('card clearfix ep-card', { 'selected': node.selected })}>
                {
                    imageTypes.indexOf(node.contentType) < 0 ?
                        <div><img class="img-fluid card-img-top"
                            src={require('../../../assets/images/480px-Icons8_flat_folder.png')} />
                            <div class="ep-checkbox selection">
                                <input id={node.id}
                                    type="checkbox"
                                    name='selectFolder'
                                    checked={node.selected}
                                    onChange={(event) => this.handleInputChange(event, node)}
                                    onClick={this.handleInputClick} />
                                <label for="selectFolder"
                                    onClick={(event) => this.triggerSelection(event, node)}></label>
                            </div>
                        </div>
                        :
                        <img class="img-fluid card-img-top"
                            src={API.getServerDomain() + node.virtualPath} />
                }
                <div class='card-block'>
                    <h4 class='card-title'>{node.name}</h4>
                </div>
            </div>);

        if (node.nodeType == 1) { // folder
            return (
                <div class="col-2 ep-node-item">
                    <Link to={`/files/${node.id}`}>{nodeItem}</Link>
                </div>
            );
        } else {
            return (nodeItem);
        }
    }

    render() {
        const {
            files,
            currentPage,
            pages,
            showPagination,
            blob
        } = this.props.fileState;

        const imageTypes = ['image/gif', "image/jpeg", "image/png"];
        return (
            <section class='container'>
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
