import { h, Component } from 'preact';
import {
    connect
} from 'preact-redux';

import {
    bindActionCreators
} from 'redux';
import { Text } from 'preact-i18n';
let classNames = require('classnames');

import { askToShowCreateFolderDialog } from '../../folders/actions';
import { hitToBrowseFile } from '../upload/actions';
import { getFiles } from '../fileActions';

import Uploader from '../upload/container';
import CreateFolder from '../../folders/create/container';

class FileMenu extends Component {
    render() {
        const { askToShowCreateFolderDialog,
            hitToBrowseFile,
            params
        } = this.props;
        return (
            <div>
                <nav class="navbar navbar-expand-lg navbar-light bg-light rounded mb-3">
                    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class='collapse navbar-collapse' id='navbarSupportedContent'>
                        <ul class="navbar-nav text-md-center nav-justified w-100">
                            <li class="nav-item active">
                                <button class="btn btn-primary nav-link ep-nav-link" type='button'
                                    onClick={() => askToShowCreateFolderDialog(params.id)}>
                                    <i class='material-icons'>add</i>
                                    <Text id='files.createFolder'></Text>
                                </button>
                            </li>
                            <li class="nav-item">
                                <button class="btn btn-primary nav-link ep-nav-link" type='button'
                                    onClick={() => hitToBrowseFile()}>
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
                <CreateFolder callbackAction={getFiles} />
                <Uploader />
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
        hitToBrowseFile: () => {
            dispatch(hitToBrowseFile());
        }
    }
}
export default connect(mapStateToProps, mapDispatchToProps)(FileMenu);
