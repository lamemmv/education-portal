import { h, Component } from "preact";
import { connect } from 'preact-redux';
import { Localizer, Text } from 'preact-i18n';
import {
    bindActionCreators
} from 'redux';
import question from '../../../../assets/images/orange-question-mark-icon.png';
import * as modalActions from './actions';

class EPConfirmation extends Component {

    componentWillReceiveProps(nextProps) {
        if (nextProps.modal.showDialog) {
            $(this.confirmationDialog).modal({ backdrop: 'static' });
        } else {
            if (this.confirmationDialog.className.indexOf('show') >= 0) {
                $(this.confirmationDialog).modal('hide');
            }
        }
    }

    render() {
        const { action, callbackAction, request } = this.props;
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
                                onClick={() => action({
                                    request: request,
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
        modal: state.dialog
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(modalActions, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(EPModal);
