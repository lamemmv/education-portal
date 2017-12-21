import { h, Component } from "preact";
import * as styles from './upload.css';

const file_input_div = {
    margin: 'auto',
    width: '250px',
    height: '40px',
}

const epButton = {
    borderRadius: '50%',
    minWidth: '40px',
    width: '40px',
    height: '40px'
}

class Upload extends Component {

    selectFile(file) {
        let reader = new FileReader();
        reader.onloadend = () => {
            file.url = reader.result;
            this.props.selectFile(file);
        }
        reader.readAsDataURL(file);
    }

    render() {
        let fileInput = null;
        const uid = 'testinput';
        const { removeFile, selectFile, uploadFile } = this.props;
        const { files } = this.props.uploadState;
        const supportedImages = ['png', 'jpg', 'gif', 'jpeg'];
        return (
            <div class="ep-wrapper">
                {/* <div class="fixed-action-btn horizontal" style="position: absolute; display: inline-block; right: 19px;">
                    <a class="btn-floating btn-large red">
                        <i class="mdi-social-notifications-none"></i>
                    </a>
                    <ul>
                        <li><a class="btn-floating red"><i class="large mdi-editor-insert-chart"></i></a>
                        </li>
                        <li><a class="btn-floating yellow darken-1"><i class="large mdi-editor-format-quote"></i></a>
                        </li>
                        <li><a class="btn-floating green"><i class="large mdi-editor-publish"></i></a>
                        </li>
                        <li><a class="btn-floating blue"><i class="large mdi-editor-attach-file"></i></a>
                        </li>
                    </ul>
                </div> */}
                <div style={file_input_div}>
                    <div style={{ float: 'left', marginTop: '10px' }}>
                        <label class="mdc-button mdc-button--raised" style={epButton} >
                            <i class="material-icons mdc-button__icon" style={{
                                position: 'absolute',
                                right: '2px'
                            }}>file_upload</i>
                            <input id="file_input_file" style={{ display: 'none' }} type="file"
                                type="file" id={uid}
                                style={{ display: "none" }}
                                onChange={() => {
                                    this.selectFile(fileInput.files[0]);
                                }}
                                ref={input => {
                                    fileInput = input;
                                }} />
                        </label>
                    </div>
                </div>
                <div class="mdc-list-group ep-group">
                    <ul class='mdc-list mdc-list--two-line mdc-list--avatar-list'>
                        {files.map(file => {
                            return (
                                <li class="mdc-list-item">
                                    <span class="mdc-list-item__start-detail ep-list-item" role="presentation">
                                        <i class="material-icons" aria-hidden="true">insert_drive_file</i>
                                    </span>
                                    <span class="mdc-list-item__text">
                                        {file.name}
                                        <span class="mdc-list-item__secondary-text">
                                            {file.lastModifiedDate}
                                        </span>
                                    </span>
                                    <button class="mdc-button mdc-ripple-upgraded mdc-list-item__end-detail"
                                        style={epButton}
                                        onClick={() => removeFile(file)}
                                        title='Delete'>
                                        <i class="material-icons mdc-button__icon"
                                            style={{
                                                position: 'absolute',
                                                right: '2px',
                                                top: '2px'
                                            }}>clear</i>
                                    </button>
                                </li>
                            )
                        })}
                    </ul>
                </div>
                <button class="mdc-button mdc-button--raised"
                    onClick={() => uploadFile(files)}
                    disabled={files.length == 0} >
                    Upload all
                </button>
            </div>
        );
    };
}

export default Upload;
