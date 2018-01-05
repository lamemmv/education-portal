import { h, Component } from "preact";
import { Link } from 'react-router-dom';
import { connect } from 'preact-redux';
import { Localizer, Text } from 'preact-i18n';
import API from '../api';

import {
    getFiles
} from './fileActions';

import { askForDeleting } from './delete/actions';
import { askToShowCreateFolderDialog } from '../folders/actions';

import Uploader from './upload/container';
import Pagination from './pagination/container';
import DeleteFile from './delete/container';
import NotificationContainer from '../notify/notification.container';
import CreateFolder from '../folders/create/container';
import * as styles from './styles.css';
import folderIcon from '../../../assets/images/folder_icon_green.png';

class FileList extends Component {
    componentWillReceiveProps(nextProps) {
        //this.initialize();
    }

    componentWillMount() {
        this.init();
    }

    init = () => {
        if (!this.props.fileState.files) {
            this.props.fileState.files = [];
        }
        this.props.getFiles(this.props.fileState.currentPage);
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
            <div class='card clearfix'>
                {
                    imageTypes.indexOf(node.contentType) < 0 ?
                        <img class="img-fluid card-img-top"
                            src={require('../../../assets/images/480px-Icons8_flat_folder.png')} />
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
                <div class="col-3 ep-node-item"><Link to={`/files/${node.id}`}>{nodeItem}</Link></div>
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
            showPagination
        } = this.props.fileState;
        const { askToShowCreateFolderDialog } = this.props;
        const imageTypes = ['image/gif', "image/jpeg", "image/png"];
        return (
            <section class='container'>
                <nav class="navbar navbar-expand-lg navbar-light bg-light rounded mb-3">
                    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class='collapse navbar-collapse' id='navbarSupportedContent'>
                        <ul class="navbar-nav text-md-center nav-justified w-100">
                            <li class="nav-item active">
                                <button class="btn btn-primary nav-link ep-nav-link" type='button'
                                    onClick={() => askToShowCreateFolderDialog()}>
                                    <i class='material-icons'>add</i>
                                    <Text id='files.createFolder'></Text>
                                </button>
                            </li>
                            <li class="nav-item">
                                <button class="btn btn-primary nav-link ep-nav-link" type='button'>
                                    <i class='material-icons'>add_to_queue</i>
                                    <Text id='files.selectFile'></Text>
                                </button>
                            </li>
                            <li class="nav-item">
                                <button class="btn btn-primary nav-link ep-nav-link" type='button'>
                                    <i class='material-icons'>arrow_forward</i>
                                    <Text id='files.moveTo'></Text>
                                </button>
                            </li>
                            <li class="nav-item">
                                <button class="btn btn-primary nav-link ep-nav-link" type='button'>
                                    <i class='material-icons'>content_copy</i>
                                    <Text id='files.copyTo'></Text>
                                </button>
                            </li>
                            <li class="nav-item">
                                <button class="btn btn-primary nav-link ep-nav-link" type='button'>
                                    <i class='material-icons'>mode_edit</i>
                                    <Text id='files.rename'></Text>
                                </button>
                            </li>
                        </ul>
                    </div>
                </nav>
                {/* <NotificationContainer />
                <Uploader /> */}
                <div class='row'>
                    {
                        files.map((node) => {
                            return this.renderNode(node)
                        })
                    }
                </div>
                {/* <DeleteFile />
                {this.renderTableFooter(pages, currentPage, showPagination)} */}
                <CreateFolder />
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
        getFiles: (page) => {
            dispatch(getFiles(page));
        },
        askForDeleting: (id) => {
            dispatch(askForDeleting(id));
        },
        askToShowCreateFolderDialog: () => {
            dispatch(askToShowCreateFolderDialog());
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(FileList);
