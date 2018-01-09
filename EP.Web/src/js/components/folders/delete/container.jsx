import { h, Component } from "preact";
import { connect } from 'preact-redux';
import { Localizer, Text } from 'preact-i18n';
import {
    bindActionCreators
} from 'redux';
import question from '../../../../assets/images/orange-question-mark-icon.png';
import * as FolderActions from '../actions';

class DeleteFolder extends Component {

    constructor(props, context) {
        super(props, context);
    }

    componentWillReceiveProps(nextProps) {
        if (nextProps.folders.showDeleteFolderDialog) {
            $(this.deleteFolderDialog).modal({ backdrop: 'static' });
        } else {
            if (this.deleteFolderDialog.className.indexOf('show') >= 0) {
                $(this.deleteFolderDialog).modal('hide');
            }
        }
    }

    render() {
        const { id, parent, nodes } = this.props.folders.request;
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
                            <img src={question} height={50} />
                            <span><Text id='messages.askToDeleteFolder'></Text></span>
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
                                        id: id,
                                        parent: parent,
                                        nodes: nodes
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
