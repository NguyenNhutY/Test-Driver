import React from 'react';
import video from './../assets/p6-re.mp4';
import '../styles/possibility.css';

const Possibility = () => (
  <div className="possibility section__padding" id="possibility">
    <div className="possibility-image">
      <video autoPlay loop muted>
        <source src={video} type="video/mp4" />
      </video>
    </div>
    <div className="possibility-content">
      <h4>Đảm bảo các bạn đủ <b>điều kiện dự thi </b>  cũng như chuẩn bị đầy đủ <b>hồ sơ</b> trước khi đăng ký thi.</h4>
      <h1 className="heading">Lợi ích của việc làm bài thi online</h1>
      <ol>
        <li>Làm bài online tại gplx.com giúp các bạn nắm vững kiến thức lý thuyết trước khi bước vào thi thật.</li>
        <br></br>
        <li>Giúp thí sinh nắm được rõ cấu trúc đề thi, các dạng câu hỏi xuất hiện.</li>
        <br></br>
        <li>Giúp các thí sinh làm quen với thời gian làm bài thi để căn chỉnh thời gian cho phù hợp.</li>
        <br></br>
        <li>Làm quen với tâm lý thi trước khi bắt đầu thi thật.</li>
        <br></br>
        <li>Thuộc lòng các câu hỏi để làm dễ dàng trong đề thi thật.</li>
        <br></br>
        <li>Nắm chắc và tránh sai các câu hỏi điểm liệt trong bài thi .</li>
      </ol>
      <a href="https://youtu.be/o6THhXDFmtA">MẸO LUYỆN THI LÝ THUYẾT NHANH</a>
    </div>
  </div>
);

export default Possibility;
