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
    }

    componentWillMount() {

    }

    selectFile(file) {
        const { single } = this.props;
        if (file) {
            let reader = new FileReader();
            reader.onloadend = () => {
                file.url = reader.result;
                this.props.selectFile({ file: file, single: single });
            }
            reader.readAsDataURL(file);
        }
    }

    render() {
        let fileInput = null;
        const uid = 'testinput';
        const { removeFile, selectFile, uploadFile, callbackAction } = this.props;
        const { files, browseFile } = this.props.uploadState;
        const supportedImages = ['png', 'jpg', 'gif', 'jpeg'];
        return (
            <div>
                <label class="custom-file">
                    <input type="file" id="file-input"
                        style={{ display: 'none' }}
                        class="custom-file-input"
                        onChange={() => {
                            this.selectFile(fileInput.files[0]);
                        }}
                        ref={input => {
                            fileInput = input;
                        }} />
                    <span class="custom-file-control"></span>
                </label>
                <ul class='list-group'>
                    {files.map(file => {
                        return (
                            <li class="list-group-item">
                                <i class="material-icons" aria-hidden="true">insert_drive_file</i>
                                <span>
                                    {file.name}
                                    {/* <span>
                                        {file.lastModifiedDate.toString()}
                                    </span> */}
                                </span>
                                <Localizer>
                                    <button type="button"
                                        class="btn btn-danger bmd-btn-fab bmd-btn-fab-sm"
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
                            </li>
                        )
                    })}
                </ul>
                {files.length > 0 ? (<button type="button"
                    class="btn btn-raised btn-primary"
                    onClick={() => uploadFile({ files: files, callbackAction: callbackAction })}
                    disabled={files.length == 0}>
                    <Text id='files.uploadAll'></Text>
                </button>) : null}
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
