import { h, Component } from "preact";
import { Link } from 'react-router-dom';
import { Localizer, Text } from 'preact-i18n';
import Uploader from './upload/container';
import Pagination from './pagination/container';
import DeleteFile from './delete/container';
import NotificationContainer from '../notify/notification.container';
import * as styles from './styles.css';

class FileList extends Component {

    componentWillMount() {
        if (!this.props.fileState.files) {
            this.props.fileState.files = [];
        }
        this.props.getFiles(this.props.fileState.currentPage);
    }

    renderTableFooter(pages, currentPage, showPagination) {
        return (
            showPagination ? (
                <Pagination />
            ) : null
        );
    }

    render() {
        const {
            files,
            currentPage,
            pages,
            showPagination
        } = this.props.fileState;
        const { askForDeleting } = this.props;
        return (
            <section class='ep-container'>
                <NotificationContainer />
                <Uploader />
                <div class="mdc-grid-list mdc-grid-list--with-icon-align-start">
                    <ul class="mdc-grid-list__tiles">
                        {files.map((file) => {
                            return (
                                <li class="mdc-grid-tile">
                                    <Link to={`file/${file.id}`}>
                                        <div class="mdc-grid-tile__primary">
                                            <img class="mdc-grid-tile__primary-content"
                                                src={require('../../../assets/images/image.png')} />
                                        </div>
                                    </Link>
                                    <span class="mdc-grid-tile__secondary">
                                        <Localizer>
                                            <i class="mdc-grid-tile__icon material-icons"
                                                title={<Text id='files.deleteFile'></Text>}
                                                onClick={() => askForDeleting(file.id)}>clear</i>
                                        </Localizer>
                                        <Link to={`file/${file.id}`}>
                                            <span class="mdc-grid-tile__title">{file.fileName}</span>
                                        </Link>
                                    </span>

                                </li>
                            )
                        })
                        }
                    </ul>
                </div>
                <DeleteFile />
                {this.renderTableFooter(pages, currentPage, showPagination)}
            </section>
        );
    };
}

export default FileList;
