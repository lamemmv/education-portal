import { h, Component } from 'preact';
import {
    connect
} from 'preact-redux';

import {
    bindActionCreators
} from 'redux';

import { Carousel } from 'react-responsive-carousel';
import * as NewsActions from '../../news/newsActions';
import * as styles from './styles.css';
import API from '../../api';

class ShortNews extends Component {
    componentWillMount() {
        this.props.actions.getNews({ page: 1, size: 6 });
    }

    render() {
        let { items } = this.props.news;
        return (
            // <Carousel
            //     showThumbs={false}
            //     autoPlay={true}
            //     showStatus={false}
            //     transitionTime={1000}
            //     interval={5000}>
            //     {items.map((item) => {
            //         return (
            //             <div class="mdc-card ep-card">
            //                 <div class="mdc-card__horizontal-block">
            //                     <section class="mdc-card__primary">
            //                         <h1 class="mdc-card__title mdc-card__title--large">{item.title}</h1>
            //                         <h2 class="mdc-card__subtitle">{item.ingress} </h2>
            //                     </section>
            //                     {
            //                         item.blob ?
            //                             (<img class="mdc-card__media-item" src={API.getServerDomain() + item.blob.virtualPath} />)
            //                             : null
            //                     }
            //                 </div>
            //                 <section class="mdc-card__actions" style={{ padding: '8px' }}>
            //                     <button class="mdc-button mdc-button--compact mdc-card__action">Chi tiết >></button>
            //                 </section>
            //             </div>                        
            //         )
            //     })}
            // </Carousel>

            <div id="myCarousel" class="carousel slide" data-ride="carousel">
                <ol class="carousel-indicators">
                    <li data-target="#myCarousel" data-slide-to="0" class=""></li>
                    <li data-target="#myCarousel" data-slide-to="1" class="active"></li>
                    <li data-target="#myCarousel" data-slide-to="2" class=""></li>
                </ol>
                <div class="carousel-inner">
                    <div class="carousel-item">
                        {/* <img class="first-slide" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="First slide" /> */}
                        <div class="container">
                            <div class="carousel-caption d-none d-md-block text-right">
                                <h1>Thông báo khai giảng khóa học mới</h1>
                                <p>Khai giảng lớp luyện thi tuyển sinh <strong>10</strong>.</p>
                                <p><a class="btn btn-lg btn-primary" href="#" role="button">Chi tiết</a></p>
                            </div>
                        </div>
                    </div>
                    <div class="carousel-item active">
                        {/* <img class="second-slide" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="Second slide" /> */}
                        <div class="container">
                            <div class="carousel-caption d-none d-md-block text-right">
                                <h1>Thông báo nghỉ lễ</h1>
                                <p>Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.</p>
                                <p><a class="btn btn-lg btn-primary" href="#" role="button">Chi tiết</a></p>
                            </div>
                        </div>
                    </div>
                    <div class="carousel-item">
                        {/* <img class="third-slide" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="Third slide" /> */}
                        <div class="container">
                            <div class="carousel-caption d-none d-md-block text-right">
                                <h1>Thông báo đóng học phí</h1>
                                <p>Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.</p>
                                <p><a class="btn btn-lg btn-primary" href="#" role="button">Chi tiết</a></p>
                            </div>
                        </div>
                    </div>
                    <div class="carousel-item">
                        {/* <img class="third-slide" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="Third slide" /> */}
                        <div class="container">
                            <div class="carousel-caption d-none d-md-block text-right">
                                <h1>Thông báo tuyển giáo viên</h1>
                                <p>Cần tuyển <strong>10</strong> giáo viên Toán, 5 giáo viên Lý, 5 giáo viên Hóa, 2 giáo viên Ngữ văn. Tốt nghiệp đại học, có ít nhất 2 năm kinh nghiệm.</p>
                                <p><a class="btn btn-lg btn-primary" href="#" role="button">Chi tiết</a></p>
                            </div>
                        </div>
                    </div>
                </div>
                <a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev">
                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                    <span class="sr-only">Previous</span>
                </a>
                <a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next">
                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                    <span class="sr-only">Next</span>
                </a>
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        news: state.news
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(NewsActions, dispatch)
    }
}


export default connect(mapStateToProps, mapDispatchToProps)(ShortNews);