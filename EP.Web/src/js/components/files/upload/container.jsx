import { h, Component } from "preact";
import {
    connect
} from 'preact-redux'
import {
    selectFile,
    uploadFiles,
    removeFile
} from './actions';
import { Localizer, Text } from 'preact-i18n';
import * as styles from './styles.css';

class Upload extends Component {

    componentWillReceiveProps(nextProps) {
        if (nextProps.uploadState.browseFile) {
            $('#file-input').trigger('click');
        }

        if (nextProps.uploadState.files.length > 0) {
            if (this.uploadFilesDialog.className.indexOf('show') < 0) {
                $(this.uploadFilesDialog).modal({ backdrop: 'static' });
            }
        } else {
            if (this.uploadFilesDialog.className.indexOf('show') >= 0) {
                $(this.uploadFilesDialog).modal('hide');
            }
        }
    }

    selectFile(files) {
        const { single } = this.props;
        const { parent } = this.props.uploadState;
        if (files) {
            // Array.from(files).map((file)=>{
            //     let reader = new FileReader();
            //     reader.onloadend = () => {
            //         file.url = reader.result;
            //         this.props.selectFile({ file: file, single: single });
            //     }
            //     reader.readAsDataURL(file);
            // });  

            this.props.selectFile({ files: files, parent: parent, single: single });
        }
    }

    render() {
        let fileInput = null;
        const { removeFile, selectFile, uploadFile, callbackAction } = this.props;
        const { files, browseFile, parent } = this.props.uploadState;
        const supportedImages = ['png', 'jpg', 'gif', 'jpeg'];
        return (
            <div class="modal fade"
                id="uploadFileModal"
                tabindex="-1"
                role="dialog"
                aria-hidden="true"
                ref={dialog => {
                    this.uploadFilesDialog = dialog;
                }}>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                        </div>
                        <div class="modal-body">
                            <div>
                                <label class="custom-file">
                                    <input type="file" id="file-input" multiple
                                        style={{ display: 'none' }}
                                        class="custom-file-input"
                                        onChange={() => {
                                            this.selectFile(fileInput.files);
                                        }}
                                        ref={input => {
                                            fileInput = input;
                                        }} />
                                    <span class="custom-file-control"></span>
                                </label>
                                <ul class='list-group'>
                                    {files.map(file => {
                                        return (
                                            <li class="list-group-item ep-list-group-item row">
                                                <div class='col-12'>
                                                    <i class="material-icons" aria-hidden="true">insert_drive_file</i>
                                                    <span>
                                                        {file.name}
                                                    </span>
                                                    <Localizer>
                                                        <button type="button"
                                                            class="btn btn-danger bmd-btn-fab bmd-btn-fab-sm float-right"
                                                            onClick={() => removeFile(file)}
                                                            title={<Text id='delete'></Text>}
                                                            style={{
                                                                width: 25,
                                                                height: 25,
                                                                minWidth: 25,
                                                                marginLeft: 50
                                                            }}>
                                                            <i class="material-icons" style={{
                                                                position: 'relative',
                                                                left: 6,
                                                                fontSize: 12
                                                            }}>clear</i>
                                                        </button>
                                                    </Localizer>
                                                </div>
                                            </li>
                                        )
                                    })}
                                </ul>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button"
                                class="btn btn-secondary"
                                data-dismiss="modal">
                                <Text id='folders.close'></Text>
                            </button>
                            {files.length > 0 ? (<button type="button"
                                class="btn btn-raised btn-primary"
                                onClick={() => uploadFile({
                                    files: files,
                                    parent: parent,
                                    action: callbackAction
                                })}
                                disabled={files.length == 0}>
                                <Text id='files.uploadAll'></Text>
                            </button>) : null}
                        </div>
                    </div>
                </div>
            </div>
        );
    };
}

const mapStateToProps = (state) => {
    return {
        uploadState: state.uploadFile.uploadState
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        selectFile: (file) => {
            dispatch(selectFile(file));
        },
        uploadFile: (files) => {
            dispatch(uploadFiles(files));
        },
        removeFile: (file) => {
            dispatch(removeFile(file))
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Upload);
