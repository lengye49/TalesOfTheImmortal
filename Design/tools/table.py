#!/usr/bin/python
# -*- coding: utf-8 -*-

import xlrd
import os
import shutil
import re
import sys
reload(sys)
sys.setdefaultencoding('utf8')

os.chdir(os.path.pardir)
zh_pattern = re.compile(u'[\u4e00-\u9fa5]+')
data_path = "specs"
cur_path = os.getcwd()


def contain_zh(word):
    '''
    判断传入字符串是否包含中文
    :param word: 待判断字符串
    :return: True:包含中文  False:不包含中文
    '''
    # word = word.decode()
    global zh_pattern
    match = zh_pattern.search(word)

    return match


def write_content_to_txt(content, table_name):
    '''print("write_content_to_txt")'''
    file_path = os.path.join(data_path)
    file_name = os.path.join(data_path, table_name + ".txt")
    if not os.path.exists(file_path):
        os.makedirs(file_path)
    if os.path.exists(file_name):
        os.remove(file_name)
    fp = open(file_name, "wb")
    fp.write(content)
    fp.close()


def excel2txt(file_name):
    '''print("excel2txt"+file_name)'''
    excel = xlrd.open_workbook(file_name)
    tables = excel.sheets()
    id_list = []
    key_list = []
    multiple_id_list = []
    empty_break = False
    find_table = False
    file_name_prefix = ""
    for table in tables:
        # # sheet 跳过中文标签
        file_name_prefix = file_name[file_name.rfind("/") + 1:file_name.index(".")]
        if table.name != file_name_prefix:
            continue

        find_table = True
        nrows = table.nrows
        if nrows < 5:
            continue

        content = ""
        for i in range(nrows):
            record = table.row_values(i)

            k = 0
            for cell in record:
                value = table.cell(i, k).value
                value_type = table.cell(1, k).value
                key = table.cell(2, k).value
                cell_type = table.cell(i, k).ctype


                if k == 0 and str(value).strip() == '':
                    empty_break = True
                    break

                if i == 1:
                    if value_type == '':
                        dump_error(file_name, key, "type should not be empty")

                    if value_type.find('list') != -1:
                        dump_error(file_name, key, "type should be List not list")

                    if value_type.find('dictionary') != -1:
                        dump_error(file_name, key, "type should be Dictionary not dictionary")

                    if value_type.find('String') != -1:
                        dump_error(file_name, key, "type should be string not String")

                    if value_type.find('Int') != -1:
                        dump_error(file_name, key, "type should be int not Int")

                    if value_type.find('Float') != -1:
                        dump_error(file_name, key, "type should be float not Float")

                if i == 2:
                    if key in key_list:
                        dump_error(file_name, value, "already has same key1111")
                    else:
                        key_list.append(value)

                # 根据类型进行转换
                if i >= 4:
                    if k == 0:
                        if value in id_list:
                            multiple_id_list.append(str(int(value)))
                        else:
                            id_list.append(value)

                    if value_type == 'int':
                        try:
                            value = int(value)
                        except ValueError:
                            dump_error(file_name, key, "int field has wrong value")

                    if value_type == 'float' and isinstance(value, float) == False:
                        dump_error(file_name, key, 'float field has wrong value')

                    # 处理300.0的情况
                    if value_type == 'string' and cell_type == 2:
                        if value == float(int(value)):
                            value = int(value)

                    # 处理300.0的情况
                    if value_type.find('List') != -1:
                        if cell_type == 2 and value_type.find('int') != -1:
                            try:
                                value = int(value)
                            except ValueError:
                                dump_error(file_name, key, "filed has wrong value")

                content = content + str(value) + ","

                k = k + 1

            if not empty_break:
                content = content + "\n"

        if len(multiple_id_list):
            dump_error(file_name, ','.join(multiple_id_list), "exit same Id")

        write_content_to_txt(content, file_name[file_name.rfind('/') + 1: file_name.find('.')])
        break

    if not find_table:
        print file_name + " not has sheet with name of " + file_name_prefix
        sys.exit(1)


def dump_error(file_name, key, error):
    print "spec " + file_name + " with key " + key + " " + error
    sys.exit(1)


def paths(path):
    path_collection = []
    for dir_path, dir_names, filenames in os.walk(path):
        for f in filenames:
            full_path = os.path.join(dir_path, f)
            full_path = full_path.replace('\\', '/')
            path_collection.append(full_path)
    return path_collection


def process_all():
    print("process_all")

    print("Clear Exist Txt")
    for output_file in os.listdir(data_path):
        if output_file.endswith(".txt"):
            os.remove(os.path.join(data_path, output_file))
    print("Dealing Excels")
    for f in paths(data_path):
        if f.endswith(".xlsx") and f.find('~') < 0:
            excel2txt(f)



def main():
    if len(sys.argv) == 1:
        process_all()


if __name__ == '__main__':
    main()
