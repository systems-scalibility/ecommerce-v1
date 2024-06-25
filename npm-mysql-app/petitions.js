import axios from 'axios';
import prettyMilliseconds from 'pretty-ms';

const connection = "http://localhost:5158/"

const request = axios.create({
    baseURL: connection
})

const getAllByCodeNumber = async (codeNumber) => {
    await request.get(`api/SalesOrderItem/product/${codeNumber}`)
}

const getAllByDate = async (startDate, endDate) => {
    await request
        .get(`api/SalesOrderItem/range-dates?startDate=${startDate}&endDate=${endDate}`)
}

const getAllByQuantity = async (quantity, condition) => {
    await request.get(`api/SalesOrderItem/quantity/${quantity}?condition=${condition}`)
}

const calculateTime = (start, end) => {
    let time = end - start;
    let timeStr = prettyMilliseconds(time, {separateMilliseconds: true});
    console.log(`Executed in ${timeStr}`);
}

console.log('Starting petitions...')
console.log('Starting petitions by code number...')
let start = new Date().getTime()
for (let i = 0; i < 1000; i++) {
    await getAllByCodeNumber(`P000${i + 1}`)
}
let end = new Date().getTime()
calculateTime(start, end)

console.log('Starting petitions by date...')
start = new Date().getTime()
for (let i = 0; i < 100; i++) {
    await getAllByDate('2024-06-25 04:53:17', '2024-06-25 04:59:17')
}
end = new Date().getTime()
calculateTime(start, end)

console.log('Starting petitions by quantity...')
start = new Date().getTime()
for (let i = 0; i < 1000; i++) {
    await getAllByQuantity(Math.floor(Math.random() * 10) + 1, '=')
}
end = new Date().getTime()
calculateTime(start, end)

connection.end();