import { h, Component } from "preact";
import { MDCDialog } from '@material/dialog';
class DeleteModal extends Component {
    componentDidMount() {
        this.dialog = new MDCDialog(this.deleteDialog);
    }

    componentWillReceiveProps(props) {
        if (this.dialog) {
            if (props.deleteState.showDeleteConfirmation) {
                this.dialog.show();
            }

            else this.dialog.close()
        }
    }

    render() {
        const {
            fileTobeDeleted
        } = this.props.deleteState;
        const { closeModal, confirmDeleteFile } = this.props;
        return (
            <div>
                <aside id="delete-file-mdc-dialog"
                    ref={dialog => {
                        this.deleteDialog = dialog;
                    }}
                    class="mdc-dialog"
                    role="alertdialog"
                    aria-labelledby="my-mdc-dialog-label"
                    aria-describedby="my-mdc-dialog-description">
                    <div class="mdc-dialog__surface">
                        <header class="mdc-dialog__header">
                            <h2 id="my-mdc-dialog-label" class="mdc-dialog__header__title">
                                Use Google's location service?
                            </h2>
                        </header>
                        <section id="my-mdc-dialog-description" class="mdc-dialog__body">
                            Let Google help apps determine location. This means sending anonymous location data to Google, even when no apps are running.
                        </section>
                        <footer class="mdc-dialog__footer">
                            <button type="button"
                                class="mdc-button mdc-dialog__footer__button mdc-dialog__footer__button--cancel"
                                onClick={closeModal}>Decline</button>
                            <button type="button"
                                class="mdc-button mdc-dialog__footer__button mdc-dialog__footer__button--accept"
                                onClick={() => confirmDeleteFile(fileTobeDeleted)}>Accept</button>
                        </footer>
                    </div>
                    <div class="mdc-dialog__backdrop"></div>
                </aside>

            </div>

        );
    }
}

export default DeleteModal;