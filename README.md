# Boxes-inventory
In this project, we practice data structures with a real-life problem
the problem is the way we store packing boxes and how we know what do we have in our inventory
because boxes probably cost a fortune, we want to maximize the profit from each box and to keep ourselves the expensive ones for the next client
or even just save some space when packing

the system responsibility is to store the data about boxes in our inventory end when needed and sell it to the customer.
because we have so many customers it needs to be very efficient and fast, that's why we use a binary search tree for storage,
not just that, the system has 2 dimensions binary trees for controlling both heihget&width and depth of the box.
To maximize the efficiency of the warehouse we are storing all our boxes, we are throwing the boxes no one orders after a certain TTL by using a double-linked list.
more functions are already implemented in the data structure that can help us make more cool features so if you have a cool idea contact me
