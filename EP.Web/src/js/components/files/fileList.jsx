import { h, Component } from "preact";
import { Link } from 'react-router-dom';
import UploadIndex from './upload/index';
import Pagination from './pagination/index';
import DeleteFile from './delete/index';
import NotificationContainer from '../notify/notification.container';

class FileList extends Component {

    componentWillMount() {
        if (!this.props.fileState.files){
            this.props.fileState.files =[];
        }
        this.props.getFiles(this.props.fileState.currentPage);
    }

    componentWillReceiveProps(nextProps){
        console.log('nextProps');
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
            <section>
                <NotificationContainer />
                <UploadIndex />
                <div class="mdc-grid-list mdc-grid-list--with-icon-align-start">
                    <ul class="mdc-grid-list__tiles">
                        {files.map((file) => {
                            return (
                                <li class="mdc-grid-tile">
                                    <div class="mdc-grid-tile__primary">
                                        <img class="mdc-grid-tile__primary-content"
                                            src={require('../../../assets/images/image.png')} />
                                    </div>
                                    <span class="mdc-grid-tile__secondary">
                                        <i class="mdc-grid-tile__icon material-icons"
                                            onClick={() => askForDeleting(file.id)}>clear</i>
                                        <span class="mdc-grid-tile__title">{file.fileName}</span>
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
