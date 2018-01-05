import { h, Component } from "preact";
import { connect } from 'preact-redux';
import { Localizer, Text } from 'preact-i18n';
import {
    bindActionCreators
} from 'redux';

import * as FolderActions from '../actions';

class CreateFolder extends Component {

    componentWillMount() {

    }

    componentWillReceiveProps(nextProps) {
        if (nextProps.folders.showDialog) {
            $(this.createFolderDialog).modal({ backdrop: 'static' });
        } else {
            $(this.createFolderDialog).modal('hide');
        }
    }

    render() {
        return (
            <div class="modal fade"
                id="createFolderModal"
                tabindex="-1"
                role="dialog"
                aria-labelledby="exampleModalLabel"
                aria-hidden="true"
                ref={dialog => {
                    this.createFolderDialog = dialog;
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
                                        <input
                                            type="text"
                                            class="form-control"
                                            placeholder={<Text id='folders.enterName'></Text>} id="folder-name" />
                                    </Localizer>

                                </div>
                            </form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal"><Text id='folders.close'></Text></button>
                            <button type="button" class="btn btn-primary"><Text id='folders.create'></Text></button>
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

export default connect(mapStateToProps, mapDispatchToProps)(CreateFolder);
