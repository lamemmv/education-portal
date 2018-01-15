import { h, Component } from "preact";
import { Localizer, Text } from 'preact-i18n';
import question from '../../../assets/images/orange-question-mark-icon.png';

class EPConfirmation extends Component {

    componentWillReceiveProps(nextProps) {
        if (nextProps.showDialog) {
            $(this.confirmationDialog).modal({ backdrop: 'static' });
        } else {
            if (this.confirmationDialog.className.indexOf('show') >= 0) {
                $(this.confirmationDialog).modal('hide');
            }
        }
    }

    render() {
        const { request, action, callbackAction } = this.props;
        return (
            <div class="modal fade"
                id="confirmationModal"
                tabindex="-1"
                role="dialog"
                aria-labelledby="deleteFolderDialog"
                aria-hidden="true"
                ref={dialog => {
                    this.confirmationDialog = dialog;
                }}>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                        </div>
                        <div class="modal-body">
                            <img src={question} height={50} />
                            <span>{request.message}</span>
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

export default EPConfirmation;
