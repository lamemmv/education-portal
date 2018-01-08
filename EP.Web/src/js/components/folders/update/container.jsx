import { h, Component } from "preact";
import { connect } from 'preact-redux';
import { Localizer, Text } from 'preact-i18n';
import {
    bindActionCreators
} from 'redux';

import * as FolderActions from '../actions';

class UpdateFolder extends Component {
    constructor(props, context) {
        super(props, context);
    }

    componentWillReceiveProps(nextProps) {
        if (nextProps.folders.showUpdateFolderDialog) {
            $(this.updateFolderDialog).modal({ backdrop: 'static' });
        } else {
            if (this.updateFolderDialog.className.indexOf('show') >= 0) {
                $(this.updateFolderDialog).modal('hide');
            }
        }

        $(this.updateFolderDialog).on('shown.bs.modal', function () {
            $('#folder-name').focus()
        });

        if (nextProps.folders.redirectTo) {
            this.context.router.history.push(nextProps.folders.redirectTo);
        }
    }

    render() {
        const { id, parent } = this.props.folders.request;
        let folderName = null;
        const { updateFolder } = this.props.actions;
        return (
            <div class="modal fade"
                id="updateFolderModal"
                tabindex="-1"
                role="dialog"
                aria-labelledby="updateFolderDialog"
                aria-hidden="true"
                ref={dialog => {
                    this.updateFolderDialog = dialog;
                }}>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                        </div>
                        <div class="modal-body">
                            <form>
                                <div class="form-group">
                                    <label for="folder-name" class="form-control-label"><Text id='folders.name'></Text></label>
                                    <Localizer>
                                        <input type="text"
                                            class="form-control"
                                            placeholder={<Text id='folders.enterName'></Text>}
                                            id="folder-name"
                                            ref={input => {
                                                folderName = input;
                                            }
                                            } />
                                    </Localizer>
                                </div>
                            </form>
                        </div>
                        <div class="modal-footer">
                            <button type="button"
                                class="btn btn-secondary"
                                data-dismiss="modal">
                                <Text id='folders.close'></Text>
                            </button>
                            <button type="button"
                                class="btn btn-primary"
                                onClick={() => updateFolder({
                                    name: folderName.value,
                                    id: id,
                                    parent: parent
                                })}>
                                <Text id='folders.update'></Text>
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

export default connect(mapStateToProps, mapDispatchToProps)(UpdateFolder);
