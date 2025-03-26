function toggleGroupSelect() {
    const taskPG = document.getElementById('taskPG');
    const groupSelect = document.getElementById('groupSelect');
    const groupNoSelect = document.getElementById('groupNoSelect');
    const hiddenInput = document.getElementById('hiddenGroupNo');
    if (taskPG.value === 'personal') {
       groupSelect.style.display = 'none'; // 隱藏小組選擇清單
        groupNoSelect.value = ''; // 將小組選擇設為空值（NULL）
    } else {
        groupSelect.style.display = 'block'; // 顯示小組選擇清單
        hiddenInput.value = groupSelect.value; // 設定選中的小組值
    }
}
//// 監聽小組選擇變更，確保值被更新
//    document.getElementById('groupNoSelect').addEventListener('change', function () {
//    document.getElementById('hiddenGroupNo').value = this.value;)}
;

// 預防初始狀態錯誤
document.addEventListener("DOMContentLoaded", toggleGroupSelect);

function updateEndDateMin() {

    const startDateInput = document.getElementById("Task_TaskStartDate");
    const endDateInput = document.getElementById("Task_TaskEndDate");
    console.log("startDateInput:", startDateInput);
    console.log("endDateInput:", endDateInput);
    console.log("startDate value:", startDateInput?.value);
        if (startDateInput.value) {
            endDateInput.min = startDateInput.value;
            console.log("Set endDate min to:", endDateInput.min);
        }
    }


document.addEventListener("DOMContentLoaded", function () {

    toggleGroupSelect();
    updateGroupNo();

    const startDateInput = document.getElementById("Task_TaskStartDate");


    if (startDateInput) {
        updateEndDateMin();
        startDateInput.addEventListener("change", updateEndDateMin);
    } else {
        console.error("Task_TaskStartDate not found on page load");
    }
});