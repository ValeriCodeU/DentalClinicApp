$(function () {

	$('.datepicker').datepicker(
		{
			minDate: new Date(),
			//maxDate: AddSubstractMonths(new Date(), 2)
        }		
	);

	//function AddSubstractMonths(date, numMonths) {

	//	var month = getMonth();

	//	var miliSeconds = new Date(date).setMonth(month + numMonths);

	//	return new Date(miliSeconds);
 //   }


});