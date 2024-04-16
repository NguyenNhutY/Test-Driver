export const Chooses = ({ ques, handleChoose, currentQuestionIndex }) => {
    return (
        ques && ques.Dapans.$values.map((value, index) => (
            <div onClick={() => handleChoose(currentQuestionIndex, index)} key={index} className="choose-val w-full p-2 flex items-center space-x-4 border-solid m-2 border border-2">
                <span className="index-span">{index + 1}</span>
                <input type="radio" className="btn-check" name="question" />
                <label className="ml-2 w-100" htmlFor={`choice${currentQuestionIndex}-${index + 1}`}>{value.Dapan1}</label>
            </div>
        ))
    );
}