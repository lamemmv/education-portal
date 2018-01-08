import { h, Component } from "preact";
import { connect } from 'preact-redux';
import { Localizer, Text } from 'preact-i18n';
import {
    bindActionCreators
} from 'redux';

import * as FolderActions from '../actions';

class DeleteFolder extends Component {

    componentWillReceiveProps(nextProps) {
        if (nextProps.folders.showDeleteFolderDialog) {
            $(this.deleteFolderDialog).modal({ backdrop: 'static' });
        } else {
            $(this.deleteFolderDialog).modal('hide');
        }
    }

    render() {
        const { folderId } = this.props.folders;
        const { deleteFolder } = this.props.actions;
        const { callbackAction } = this.props;
        return (
            <div class="modal fade"
                id="deleteFolderModal"
                tabindex="-1"
                role="dialog"
                aria-labelledby="deleteFolderDialog"
                aria-hidden="true"
                ref={dialog => {
                    this.deleteFolderDialog = dialog;
                }}>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                        </div>
                        <div class="modal-body">
                            <i class='material-icons'>warning</i>
                            <p><Text id='message.askToDeleteFolder'></Text></p>
                        </div>
                        <div class="modal-footer">
                            <button type="button"
                                class="btn btn-secondary"
                                data-dismiss="modal">
                                <Text id='cancel'></Text>
                            </button>
                            <button type="button"
                                class="btn btn-primary"
                                onClick={() => deleteFolder({
                                    request: {
                                        id: folderId
                                    },
                                    action: callbackAction
                                })}>
                                <Text id='ok'></Text>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        );
    };
}

const mapStateToProps = (state) => {
    return {
        folders: state.folders
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(FolderActions, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(DeleteFolder);
