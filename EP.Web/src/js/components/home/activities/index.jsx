import { h, Component } from 'preact';

import { Carousel } from 'react-responsive-carousel';

import * as styles from './styles.css';
import activity1 from '../../../../assets/images/news/2.jpeg';
import activity2 from '../../../../assets/images/news/3.jpeg';
import activity3 from '../../../../assets/images/news/4.jpeg';
import activity4 from '../../../../assets/images/activity-4.jpg';
import activity5 from '../../../../assets/images/activity-5.jpg';
import activity6 from '../../../../assets/images/activity-6.jpg';
import activity7 from '../../../../assets/images/activity-7.jpg';
import activity8 from '../../../../assets/images/activity-8.jpg';
import activity9 from '../../../../assets/images/activity-9.jpg';
import teachingMethod from '../../../../assets/images/teaching-methods.jpg';
import rules from '../../../../assets/images/rules.jpg';
import money from '../../../../assets/images/Money-Bag.jpg';

const width = {
    width: {
        value: '100%',
        important: 'true'
    }
}
class EPActivities extends Component {
    render() {
        return (
            // <Carousel
            //     showThumbs={false}
            //     autoPlay={true}
            //     showStatus={false}
            //     transitionTime={1000}
            //     interval={5000}
            //     axis="vertical">
            //     <div>
            //         <img src={activity1}
            //             ref={(node) => {
            //                 if (node) {
            //                     node.style.setProperty("width", "100%", "important");
            //                 }
            //             }} />
            //     </div>
            //     <div>
            //         <img src={activity2} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity3} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity4} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity5} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity6} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity7} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity8} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity9} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            // </Carousel>
            <div class="container marketing">
                <div class="row">
                    <div class="col-lg-4">
                        <img class="rounded-circle" src={teachingMethod} alt="Generic placeholder image" width="140" height="140" />
                        <h2>Phương pháp & chương trình dạy</h2>
                        <p>Nâng cao tính tích cực của học viên</p>
                        <p><a class="btn btn-secondary" href="#" role="button">Chi tiết »</a></p>
                    </div>
                    <div class="col-lg-4">
                        <img class="rounded-circle" src={rules} alt="Generic placeholder image" width="140" height="140" />
                        <h2>Nội qui trung tâm</h2>
                        <p>Trang bị cho học sinh kiến thức và tính kỹ luật trong học tập</p>
                        <p><a class="btn btn-secondary" href="#" role="button">Chi tiết »</a></p>
                    </div>
                    <div class="col-lg-4">
                        <img class="rounded-circle" src={money} alt="Generic placeholder image" width="140" height="140" />
                        <h2>Qui định học phí</h2>
                        <p>Có chính sách miễn giảm học phí cho học viên có phụ huynh là giáo viên, gia đình có hoàn cảnh khó khăn</p>
                        <p><a class="btn btn-secondary" href="#" role="button">Chi tiết »</a></p>
                    </div>
                </div>
                <hr class="featurette-divider" />
                <div class="row featurette">
                    <div class="col-md-7">
                        <h2 class="featurette-heading">Cơ sở vật chất<span class="text-muted"></span></h2>
                        <p class="lead">Phòng học thoáng mát sạch sẽ, phòng máy lạnh, camera quan sát. Mỗi học viên ngồi một bàn riêng biệt.</p>
                    </div>
                    <div class="col-md-5">
                        <img class="featurette-image img-fluid mx-auto"
                            data-src="holder.js/500x500/auto"
                            alt="500x500"
                            src={activity1}
                            data-holder-rendered="true" />
                    </div>
                </div>
                <hr class="featurette-divider" />
                <div class="row featurette">
                    <div class="col-md-7 order-md-2">
                        <h2 class="featurette-heading">Các hoạt động của trung tâm</h2>
                        <p class="lead">Trung tâm thường xuyên tổ chức các buổi kiểm tra định kỳ cho các học viên mỗi tháng một lần. Có báo cáo kết quả cho phụ huynh, tổ chức khen thưởng cho các học viên đạt kết quả tốt. Cũng như có kế hoạch phụ đạo cho các học viên chưa đạt yêu cầu.</p>
                    </div>
                    <div class="col-md-5 order-md-1">
                        <img class="featurette-image img-fluid mx-auto"
                            data-src="holder.js/500x500/auto"
                            alt="500x500"
                            src={activity2}
                            data-holder-rendered="true" />
                    </div>
                </div>
                <hr class="featurette-divider" />
                <div class="row featurette">
                    <div class="col-md-7">
                        <h2 class="featurette-heading">Thành tích học tập của học viên qua các kỳ thi</h2>
                        <p class="lead"></p>
                    </div>
                    <div class="col-md-5">
                        <img class="featurette-image img-fluid mx-auto"
                            data-src="holder.js/500x500/auto"
                            alt="500x500"
                            src={activity3}
                            data-holder-rendered="true" />
                    </div>
                </div>
            </div>
        );
    }
}

export default EPActivities;