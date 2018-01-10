import { h, Component } from 'preact';
import { Link } from 'react-router-dom';
import {
    connect
} from 'preact-redux';

import {
    bindActionCreators
} from 'redux';
import { Text } from 'preact-i18n';
let classNames = require('classnames');

import {
    askToShowCreateFolderDialog,
    askToShowUpdateFolderDialog,
    askToShowDeleteFolderDialog
} from '../../folders/actions';

import { hitToBrowseFile } from '../upload/actions';
import { getFiles, hitToDownloadFile } from '../fileActions';

import Uploader from '../upload/container';
import CreateFolder from '../../folders/create/container';
import UpdateFolder from '../../folders/update/container';
import DeleteFolder from '../../folders/delete/container';

class FileMenu extends Component {
    render() {
        const { askToShowCreateFolderDialog,
            askToShowUpdateFolderDialog,
            askToShowDeleteFolderDialog,
            hitToDownloadFile,
            hitToBrowseFile,
            params
        } = this.props;

        const { files, selectedNode } = this.props.fileState;

        return (
            <div>
                <nav class="navbar navbar-expand-lg navbar-light bg-light rounded mb-3 ep-menu">
                    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class='collapse navbar-collapse' id='navbarSupportedContent'>
                        <ul class="navbar-nav w-100">
                            <li class="nav-item active">
                                <button class="btn btn-primary nav-link ep-nav-link" type='button'
                                    onClick={() => askToShowCreateFolderDialog(params.id)}>
                                    <i class='material-icons'>add</i>
                                    <Text id='files.createFolder'></Text>
                                </button>
                            </li>
                            <li class="nav-item">
                                <button class="btn btn-primary nav-link ep-nav-link" type='button'
                                    onClick={() => hitToBrowseFile(params.id)}>
                                    <i class='material-icons'>add_to_queue</i>
                                    <Text id='files.selectFile'></Text>
                                </button>
                            </li>
                            {
                                files.filter(node => node.selected == true).length >= 1 ? <li class="nav-item">
                                    <button class="btn btn-primary nav-link ep-nav-link" type='button'
                                        onClick={() => askToShowDeleteFolderDialog({
                                            nodes: files,
                                            parent: params.id
                                        })}>
                                        <i class='material-icons'>delete</i>
                                        <Text id='files.delete'></Text>
                                    </button>
                                </li> : null
                            }
                            {
                                files.filter(node => node.selected == true).length == 1 ?
                                    <li class="nav-item">
                                        <button class="btn btn-primary nav-link ep-nav-link" type='button'>
                                            <i class='material-icons'>content_copy</i>
                                            <Text id='files.copyTo'></Text>
                                        </button>
                                    </li> : null
                            }
                            {
                                files.filter(node => (node.selected == true && node.nodeType == 1)).length == 1 ?
                                    <li class="nav-item">
                                        <button class="btn btn-primary nav-link ep-nav-link" type='button'
                                            onClick={() => askToShowUpdateFolderDialog({
                                                id: selectedNode.id,
                                                name: selectedNode.name,
                                                parent: params.id
                                            })}>
                                            <i class='material-icons'>file_download</i>
                                            <Text id='files.rename'></Text>
                                        </button>
                                    </li> : null
                            }
                            {
                                files.filter(node => (node.selected == true && node.nodeType == 2)).length == 1 ?
                                    <li class="nav-item">
                                        <button class="btn btn-primary nav-link ep-nav-link" type='button'
                                            onClick={() => hitToDownloadFile(files.filter(n => (n.selected == true && n.nodeType == 2))[0].id)}>
                                            <i class='material-icons'>file_download</i>
                                            <Text id='files.download'></Text>
                                        </button>
                                    </li> : null
                            }
                        </ul>
                    </div>
                </nav>
                <CreateFolder callbackAction={getFiles} />
                <UpdateFolder callbackAction={getFiles} />
                <DeleteFolder callbackAction={getFiles} />
                <Uploader callbackAction={getFiles} />
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        fileState: state.posts.fileState
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        askToShowCreateFolderDialog: (folderId) => {
            dispatch(askToShowCreateFolderDialog(folderId));
        },
        askToShowUpdateFolderDialog: (request) => {
            dispatch(askToShowUpdateFolderDialog(request));
        },
        askToShowDeleteFolderDialog: (request) => {
            dispatch(askToShowDeleteFolderDialog(request));
        },
        hitToBrowseFile: (parent) => {
            dispatch(hitToBrowseFile(parent));
        },
        hitToDownloadFile: (id) => {
            dispatch(hitToDownloadFile(id))
        }
    }
}
export default connect(mapStateToProps, mapDispatchToProps)(FileMenu);
